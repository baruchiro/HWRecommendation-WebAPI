#!/bin/bash

cd $GITHUB_WORKSPACE/DataPreparing/src/prepare/

pip install -r ../../requirements.txt

sh -c "python $*"
