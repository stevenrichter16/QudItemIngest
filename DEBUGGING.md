# Debugging into a NuGet package (stepping through external source)

Stepping into a NuGet package's code means getting the debugger to map the
package's compiled IL back to **source lines**. That requires two things the
package's own DLL doesn't carry by default: **symbols** (a PDB mapping IL ↔
source) and the **actual source files**. Here's the whole picture, then the
concrete steps.

## The three things that must line up

1. **The right PDB** — debug symbols matching the *exact* build of the DLL you
   have. Without it the debugger only sees IL/addresses, no line numbers, no
   locals.
2. **The source files** — the original `.cs`, matching the same commit.
3. **A debugger willing to leave your code** — by default IDEs only stop in your
   own code.

Modern packages solve #1 and #2 via **Source Link** + embedded or downloadable
symbols. That's the path that "just works" when the package author enabled it
(most Microsoft/.NET and many popular packages do).

## Steps (IDE-agnostic)

### 1. Build in Debug and turn off "Just My Code"

"Just My Code" tells the debugger to skip any code without your project's
symbols — i.e., it actively prevents stepping into packages. Disable it.

- **Visual Studio:** Tools → Options → Debugging → General → uncheck *Enable
  Just My Code*.
- **Rider:** Settings → Build, Execution, Deployment → Debugger → uncheck *Skip
  external code* (Rider calls it "External Source").
- **VS Code (C# Dev Kit):** in `launch.json`, set `"justMyCode": false`.

### 2. Enable Source Link / external source debugging

Source Link is metadata baked into the PDB that says "the source for this DLL
lives at *this* git URL at *this* commit." The debugger downloads each file on
demand as you step.

- **Visual Studio:** Debugging → General → check *Enable Source Link support*
  (and uncheck *Require source files to exactly match the original* if you hit
  mismatch prompts).
- **Rider:** it offers "Download source from Source Link / symbol servers" when
  you step into external code; enable external-source debugging in the Debugger
  settings.
- **VS Code:** `"suppressJITOptimizations": true` and `justMyCode: false` enable
  it; symbol/source options live in `launch.json`.

### 3. Configure symbol sources (where PDBs come from)

Symbols arrive one of three ways — you want at least one available:

- **Embedded PDB** — the PDB is *inside* the DLL (package built with
  `<DebugType>embedded</DebugType>`). Nothing to configure; it's always there.
- **Symbol package (`.snupkg`)** on a **symbol server** — configure the server
  so the debugger can fetch PDBs:
  - **Visual Studio:** Debugging → Symbols → enable *NuGet.org Symbol Server*
    (and/or Microsoft Symbol Servers), set a local symbol cache dir.
  - **Rider:** Settings → Debugger → Symbol Servers → add
    `https://symbols.nuget.org/download/symbols`.
- **PDB shipped in the main package** — already next to the DLL in your
  `~/.nuget/packages` cache; works automatically.

### 4. Set a breakpoint and step in

- Put a breakpoint on *your* line that calls into the package.
- Use **Step Into (F11)**. The debugger resolves the symbol, sees the Source
  Link URL, downloads the matching source file, and stops on that line.
- First step-in for a given file may pause to download; afterward it's cached.

### 5. (If a prompt appears) accept the source download / mismatch

You may get "Source Link will download from github.com…" — accept it. If you
get a *checksum mismatch* warning, it means the DLL and the source/PDB are from
different builds; either you have the wrong symbols or must relax exact-match
(see step 2).

## When Source Link isn't available — fallbacks

Not every package ships symbols or Source Link. Then:

- **Decompile + debug decompiled sources.** Rider/ReSharper and Visual Studio
  can decompile the DLL to reconstructed C# and generate PDBs for it, letting
  you step through the *decompiled* code (not original, but usually close).
  - **Rider:** on by default — stepping into a symbol-less assembly decompiles
    it automatically.
  - **Visual Studio:** when you try to view a symbol-less frame, it offers
    *Decompile source code*.
  - **dotPeek** (standalone JetBrains tool) can run as a **symbol server** that
    generates PDBs on the fly for any assembly.
- **Build the package from source yourself** and use a `ProjectReference` (or a
  local package + its PDB) instead — guarantees exact symbol/source match. This
  is the most reliable but heaviest option.

## How to verify before you start

- Check whether the package has symbols: look in
  `~/.nuget/packages/<pkg>/<version>/lib/...` for a `.pdb` next to the `.dll`,
  or whether a `.snupkg` exists on nuget.org.
- A quick tell for embedded PDB + Source Link: the package's project used
  `<DebugType>embedded</DebugType>` and `<PublishRepositoryUrl>` /
  `Microsoft.SourceLink.*`.

## The mental model

> Stepping into a package = matching **DLL ⇄ PDB ⇄ source**. Source Link
> automates the source half; symbol servers/embedded PDBs supply the symbol
> half; turning off Just My Code lets the debugger actually go there. If any
> link is missing, you fall back to decompilation.
