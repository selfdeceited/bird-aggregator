Bird-Aggregator 2.0 release-ready checklist

# BACKEND
 - [ ] add data migration console app
 - [ ] swagger working with typedoc
 - [ ] structured logging
 - [ ] proper env values handling with secret storage
 - [ ] Check at app start if all appSettings params has been set correctly
 - [ ] add health checks
 - [ ] remove old service (`./bird-aggregator`)


# FRONTEND
 - [ ] fix map bug for photo and bird pages
 - [ ] switch to styled components
 - [ ] switch to effector
 - [ ] switch to vite
 - [ ] service layer (e.g. http calls)
 - [ ] convert logic to hooks
 - [ ] Fix all `smth as any`
 - [ ] create a mock server
 - [ ] run tests in CI at github actions
 - [ ] update blueprintjs
 - [ ] normal icons


# INFRA

 - [ ] CI pipeline at github actions
 - [ ] Healthcheck in Dockerfile.api
 - [ ] K U B E R N E T E S (:
 - [ ] CD in Github Actions
 - [ ] integration tests
 - [ ] deploy data migration app as a sidecar