FROM python:3

LABEL version="1.0.0"

LABEL maintainer="Baruch Rothkoff"
LABEL com.github.actions.name="Python"
LABEL com.github.actions.description="GitHub Action to run a Python sub-project in this repo"
LABEL com.github.actions.icon="triange"
LABEL com.github.actions.color="green"

COPY . .

RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]