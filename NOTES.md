# Chalkboard Save/Load BepInEx Mod - Strategy & Notes

## Overview

This mod saves and restores the state of a `QuadPainterGPU` chalkboard to/from disk,
and broadcasts the loaded state to all connected players (and future late-joiners)
without requiring any mod on the client side.

---

## Board State Architecture

The board stores its state in two places:

| Field | Type | Access | Purpose |
|---|---|---|---|
| `PaintColors` | `IntList[]` | `public` | Source of truth. 2D grid (x=320, y=180) of color indices. `0` = empty, `n+1` = chalk color index `n`. |
| `_rt` | `RenderTexture` | `private` | GPU-side visual. Rebuilt from `PaintColors`. Not persistent. |

Color indices map through `DrawingManager.GetColor(int index)`. They are palette-relative,
so saves are portable as long as the chalk palette order doesn't change between game versions.

---

## Saving

`PaintColors` is public - no Harmony reflection needed to read it.

```csharp
var board = GameObject.FindObjectOfType<QuadPainterGPU>();

// Serialize PaintColors to JSON
var json = JsonConvert.SerializeObject(board.PaintColors);
File.WriteAllText(Path.Combine(saveDir, boardName + ".board"), json);
```

### Optional: Save a PNG preview

```csharp
var rt = Traverse.Create(board).Field("_rt").GetValue<RenderTexture>();
var tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
RenderTexture.active = rt;
tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
tex.Apply();
File.WriteAllBytes(Path.Combine(saveDir, boardName + ".png"), tex.EncodeToPNG());
RenderTexture.active = null;
Object.Destroy(tex);
```

---

## Loading & Broadcasting

### Requirements
- You **must be the host** (`board.isServer == true`). See [Authority section](#authority--hosting-requirement) below.

### Steps

#### 1. Deserialize into PaintColors
```csharp
var json = File.ReadAllText(path);
board.PaintColors = JsonConvert.DeserializeObject<IntList[]>(json);
```

#### 2. Rebuild local render texture
`RenderBatch()` and `PaintPixel()` are private - access via Harmony `Traverse`:

```csharp
// Clear the render texture first
var rt = Traverse.Create(board).Field("_rt").GetValue<RenderTexture>();
RenderTexture active = RenderTexture.active;
RenderTexture.active = rt;
GL.Clear(true, true, Color.clear);
RenderTexture.active = active;

// Re-paint all non-empty cells
var paintPixel = Traverse.Create(board).Method("PaintPixel", new[] { typeof(Vector2Int), typeof(Color), typeof(int) });
for (int x = 0; x < board.PaintColors.Length; x++)
{
    for (int y = 0; y < board.PaintColors[x].Ints.Count; y++)
    {
        int idx = board.PaintColors[x].Ints[y];
        if (idx != 0)
        {
            Color color = MonoSingleton<DrawingManager>.I.GetColor(idx - 1);
            paintPixel.GetValue(new Vector2Int(x, y), color, idx);
        }
    }
}

// Flush the batch to the RenderTexture
Traverse.Create(board).Method("RenderBatch").GetValue();
```

#### 3. Broadcast to currently connected players
```csharp
foreach (var player in board.networkManager.playerManager.players)
{
    board.GetQuadImage(player, board.PaintColors);
}
```

Non-modded clients handle this natively via their compiled `HandleRPCGenerated_1`
→ `GetQuadImage_Original_1`, which fully rebuilds their canvas. **No mod required on their end.**

---

## Late-Joiner Sync (Automatic)

When a new player joins, the game already runs:

```
Client → ReadPixelData() [ServerRpc]
Server → GetQuadImage(sender, PaintColors) [TargetRpc]
Client → GetQuadImage_Original_1() → rebuilds canvas
```

Since your mod set `PaintColors` on the server, **late joiners automatically receive
the loaded board** through this existing mechanism. No extra work needed.

---

## Authority / Hosting Requirement

| RPC | Attribute | Implication |
|---|---|---|
| `GetQuadImage` | `requireServer: true` | Only the server can send this |
| `FillTheBlanksRPC` | `requireServer: true` | Only the server can broadcast this |
| `ReadPixelData` | `requireOwnership: true` | Client must own the object to call |

**You must be the host to load and sync a board.**

Always guard with:
```csharp
if (!board.isServer)
{
    Logger.LogWarning("[ChalkboardMod] Load+sync requires you to be the host.");
    return;
}
```

Saving works for everyone - it is purely a local read of `PaintColors`.

---

## Workarounds If Not Host

| Option | Notes |
|---|---|
| Save only | Always works. Read `PaintColors`, write to disk. No sync. |
| Require host mod too | Add a custom `[ServerRpc]` on a separate `NetworkBehaviour` that tells the host to load the file. Host then legitimately broadcasts. Requires both players to have the mod. |
| Harmony-patch authority check | Patch `ValidateReceivingRPC`. Fragile, likely to break on game updates. Not recommended. |

---

## Key Numbers

| Property | Value |
|---|---|
| Grid size | 320 × 180 = 57,600 cells |
| Texture size (default) | 1920 × 1080 |
| Color index 0 | Empty |
| Color index `n` | `DrawingManager.GetColor(n - 1)` |

---

## Relevant Types

- `QuadPainterGPU` - the board component (`NetworkBehaviour`)
- `IntList` - wrapper around `List<int>`, one per column
- `DrawingManager` - `MonoSingleton`, maps index → `Color`
- `PurrNet.Modules.RPCModule` - PurrNet's RPC infrastructure
- `RPCInfo` - sender metadata passed to `_Original_` methods
