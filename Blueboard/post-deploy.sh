#!/bin/bash

echo "Applying migrations"
exec ./ef-core-migrate

echo "Restarting backend"
"$(which blueboard-ctl)" restart
