### Arbitr Setup ###

# Getting Started #
## Assumptions ##
- You have Docker and Kubernetes installed on your local machine and you are using docker desktop. If you don't please go to https://docs.docker.com/get-docker/ and enable Kubernetes in the settings once Docker is installed.
- You have kubectl installed: https://kubernetes.io/docs/tasks/tools/install-kubectl/
- If you're on Windows, you have Chocolatey package manager installed; if you're on Mac, you have homebrew installed.

## Initial Setup ##
(If using the WSL2 based engine, you can skip this.)
### Allocate enough resources to Docker ###
Since we are spinning up a mini environment on our machines, you'll want to allocated enough resources for Docker.
- Right click on the Docker icon in your task bar and select Settings (Preferences for Mac Users)
- Click the Resources Menu Item
- Click the Advanced Sub Menu
- Slide the Memory slider to at least 10GB. You may want to adjust some of the other sliders to suit your individual needs as well.

### Install kustomize ###
Kustomize is a templating engine that's utilized by skaffold to help allow our manifests to be run locally

**Windows Users**
- Run `choco install kustomize` in an admin Powershell

**Mac Users**
- Run `brew install kustomize` in the terminal

### Install skaffold ###
Skaffold will be the main tool we use for development

**Windows Users**
- Run `choco install skaffold` in an admin Powershell

**Mac Users**
- Run `brew install skaffold` in the terminal

### Debugging ###
After the pods are running, you can attach to them within Visual Studio
* Debug --> Attach to Process... (Ctrl+Alt+P)
* Connection type: Docker (Linux Container)
* Click `Find` and wait for the list to populate
* Select `arbitr-api` or `arbitr-worker` accordingly

If you are running multiple replicas you might need to reduce them to one to guarantee the service you attach to will handle any requests.

# Resources
- [skaffold](https://skaffold.dev/)
- [kustomize](https://github.com/kubernetes-sigs/kustomize)
- [chocolatey](https://chocolatey.org/)
- [homebrew](brew.sh)