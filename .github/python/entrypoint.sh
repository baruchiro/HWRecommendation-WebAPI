#!/bin/bash

cd $GITHUB_WORKSPACE/DataPreparing/

export PYTHONPATH=$PYTHONPATH:$GITHUB_WORKSPACE/DataPreparing

pip install -r requirements.txt

sh -c "python $*"
