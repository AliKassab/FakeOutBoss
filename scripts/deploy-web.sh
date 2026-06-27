#!/usr/bin/env bash
# Deploy a Unity WebGL build to the gh-pages branch -> https://alikassab.dev/FakeOutBoss/
#
# Usage:
#   1. In Unity, build WebGL to the output folder below (default: ~/Documents/FakeOutBOSS).
#   2. Run:  ./scripts/deploy-web.sh
#
# Override the build folder:  BUILD_DIR=/path/to/build ./scripts/deploy-web.sh
#
# Pipeline note: this is a Built-in Render Pipeline project. Build with
# Compression Format = Gzip and Decompression Fallback = ON (GitHub Pages
# cannot set Content-Encoding headers).

set -euo pipefail

BUILD_DIR="${BUILD_DIR:-$HOME/Documents/FakeOutBOSS}"
REMOTE="https://github.com/AliKassab/FakeOutBoss.git"
BRANCH="gh-pages"

if [ ! -f "$BUILD_DIR/index.html" ]; then
  echo "ERROR: no index.html in $BUILD_DIR — build WebGL there first." >&2
  exit 1
fi

cd "$BUILD_DIR"

# First run: turn the build folder into a gh-pages working copy.
if [ ! -d .git ]; then
  git init -q
  git remote add origin "$REMOTE"
  git checkout -q --orphan "$BRANCH"
fi

# GitHub Pages: disable Jekyll so build files serve raw; never track macOS cruft.
touch .nojekyll
printf ".DS_Store\n" > .gitignore

git add -A
if git diff --cached --quiet; then
  echo "Nothing changed — build is identical to what is already deployed."
  exit 0
fi

git commit -q -m "deploy: WebGL build $(date +%Y-%m-%d_%H:%M)"
git push -f origin "$BRANCH"

echo "Deployed. Live in ~1 min at https://alikassab.dev/FakeOutBoss/"
echo "Hard-refresh (Cmd+Shift+R) to bypass CDN cache."
