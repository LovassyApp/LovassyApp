#!/bin/bash

case "$1" in

start)
    "$(which systemctl)" start blueboard
    ;;

stop)
    "$(which systemctl)" stop blueboard
    ;;

restart)
    "$(which systemctl)" restart blueboard
    ;;

*)
    STATEMENTS
    ;;
esac
