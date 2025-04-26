#!/usr/bin/env bash
set -euo pipefail

echo "⏳ Seeding feature flags into MongoDB..."

docker compose exec mongo mongosh myappdb --eval '
  db.settings.updateOne(
    {},
    { $set: { Features: { Increment: true, Decrement: true } } },
    { upsert: true }
  );
'

echo "✅ Done."
