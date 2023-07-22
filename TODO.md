**Bird-Aggregator 2.0 release-ready checklist**

# BACKEND
 - [ ] swagger working with typedoc
 - [ ] proper env values handling with secret storage
 - [ ] Check at app start if all appSettings params has been set correctly
 - [ ] `using static` all essentials
 - [ ] try auto-mappers based on source generators?

# FRONTEND
 - [ ] put wiki logic to wiki component
 - [ ] switch to styled components
 - [ ] switch to vite + vitest
 - [ ] convert logic to hooks, write tests for hooks
 - [ ] create a mock server (mjs?)
 - [ ] run tests in CI at github actions
 - [ ] update blueprintjs
 - [ ] normal icons

# INFRA

 - [ ] Robust CI & CD pipeline at github actions
 - [ ] Healthcheck in Dockerfile.api
 - [ ] K8s infra
 - [ ] integration tests
 - [ ] deploy data migration app as a sidecar, run it as a cron job
 - [ ] check out vscode devcontainer
 - [ ] add health checks
 - [ ] structured logging to time-based db - otel-exporter to prometheus + grafana cloud


 # GENERAL
  - [ ] add .editorconfig