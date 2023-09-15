#!/bin/bash

cd /data/app/Blueboard

echo "Replacing environment"
rm appsettings.Development.json
mv appsettings.Production.json appsettings.json

echo "Applying migrations"
"$(pwd)/ef-core-migrate"

echo "Restarting backend"
sudo blueboard-ctl restart
echo "Done. Backend restart successfully."
