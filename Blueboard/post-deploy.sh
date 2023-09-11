#!/bin/bash

cd /data/app/Blueboard

echo "Applying migrations"
"$(pwd)/ef-core-migrate"

echo "Restarting backend"
sudo blueboard-ctl restart
echo "Done. Backend restart successfully."
