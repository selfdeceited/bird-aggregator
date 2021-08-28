**Bird-Aggregator 2.0 release-ready checklist**

# BACKEND
 - [ ] add data migration console app
 - [ ] swagger working with typedoc
 - [ ] structured logging
 - [ ] proper env values handling with secret storage
 - [ ] Check at app start if all appSettings params has been set correctly
 - [ ] add health checks
 - [ ] remove old service (`./bird-aggregator`)


# FRONTEND
 - [ ] put wiki logic to wiki component
 - [ ] switch to styled components
 - [ ] switch to effector
 - [ ] switch to vite
 - [ ] service layer from http calls, add typegen and remove linter comments
 - [ ] convert logic to hooks, write tests for hooks
 - [ ] create a mock server
 - [ ] run tests in CI at github actions
 - [ ] update blueprintjs
 - [ ] normal icons

# INFRA

 - [ ] CI pipeline at github actions
 - [ ] Healthcheck in Dockerfile.api
 - [ ] K8s infra
 - [ ] CD in Github Actions
 - [ ] integration tests
 - [ ] deploy data migration app as a sidecar
 - [ ] vscode devcontainer