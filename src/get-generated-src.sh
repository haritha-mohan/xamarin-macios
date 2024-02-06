#!/bin/bash -e

directory_path="$1"

# FILES=$(find $(realpath "$directory_path/generated-sources") -type f -name "*.g.cs")

# # RESULT=$(echo $FILES | tr '\n' ';')
# result_variable=""

# # Iterate through each file in FILES and append to the result_variable
# for file in $FILES; do
#   result_variable="$result_variable$file;"
# done

# echo $result_variable

FILES=$(find "$(realpath "$directory_path/generated-sources")" -type f -name "*.g.cs" -exec printf "%s;" {} +)
FILES=${FILES%;}
echo $FILES