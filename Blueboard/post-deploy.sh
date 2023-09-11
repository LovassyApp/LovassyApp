#!/bin/bash

echo "Applying migrations"
exec ./ef-core-migrate

echo "Restarting backend"
sudo "$(which blueboard-ctl) restart"
