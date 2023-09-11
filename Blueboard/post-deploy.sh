#!/bin/bash

echo "Applying migrations"
./ef-core-migrate

echo "Restarting backend"
sudo "$(which blueboard-ctl) restart"
