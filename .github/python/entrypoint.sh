#!/bin/bash

pip install -r $GITHUB_WORKSPACE/DataPreparing/requirements.txt

sh -c "python $*"
