#!/bin/bash

commits=()

for commit in $(git rev-list master)
do
    if git ls-tree --name-only -r $commit | grep -q 'DataPreparing/data/fake-data-out.csv$'; then
        commits+=($commit)
        echo $commit
    fi
done

for ((i=${#commits[@]} - 1;i >= 0; i--))
do
    commit=${commits[i]}
    echo $commit
    git checkout -f $commit
    lines=$(($(wc -l DataPreparing/data/fake-data-out.csv | cut -d ' ' -f 1) - 1 ))
    echo $lines
    git tag "Data-${lines}" -m "In this commit, the data contains ${lines} rows"
    echo "git tag Data-${lines} -m In this commit, the data contains ${lines} rows"
done

git tag
