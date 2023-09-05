# WIP saga pattern demo

This project aim to show an orchestrated saga using dotnet and dapr.

## Dependencies

* Install project tye with [doc](https://github.com/dotnet/tye/blob/main/docs/getting_started.md)
* Install dapr with [doc](https://docs.dapr.io/getting-started/install-dapr-cli/)
* Install dotnet 6 with [doc](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)
* Install docker or podman

## How to run

* Start dapr with command `dapr init` or `dapr init --container-runtime podman` if you prefer podman
* Start tye with `tye run`

## How to start a saga

The file _launch-saga.sh_ is here to start a saga. The project Saga.Card have to be started as the saga start with this synchronous call.

## Simplicity

None of these services are designed to be scallable. Expect deadlock if under pressure
