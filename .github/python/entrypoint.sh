#!/bin/bash

export PYTHONPATH=$PYTHONPATH:$GITHUB_WORKSPACE/DataPreparing

pip install -r requirements.txt

sh -c "python $*"
