#!/bin/bash -e

directory_path=$1
file_list=$(find "$directory_path" -type f -printf "%f;")
file_list="${file_list%;}"
echo $file_list
