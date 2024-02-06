#!/bin/bash -e

directory_path="$1"

# # Use find's -exec to print full paths directly
FILES=$(find $(realpath "$directory_path") -type f -name "*.g.cs")

# # Join file names with semicolons
RESULT=$(echo "$FILES" | tr '\n' ';')

# echo "Result: $RESULT"
echo $RESULT
