#!/bin/bash -e

directory_path=$1
find $(realpath $directory_path) -type f | tr '\n' ';'
