dapr publish --publish-app-id card --pubsub pubsub --topic cardtopic \
  --data '{"card":{"items":[{"name":"foo","price":10},{"name":"bar","price":20}]}}'
