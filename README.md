# Service.SwaggerInKube
The service allows you to view the Swagger page for all the Pods in the indicated namespaces of the Kubernetes.

# Parameters
To run service need to specify environment variables:
* SWAGGER_KUBE_NAMESPACES = list of namespaces to search pods. Separator between namespaces - ';'  (common;services)
* WAGGER_KUBE_APIURL = Url address to kubernetes API (https://*****.hcp.westeurope.azmk8s.io:443)
* SWAGGER_KUBE_APITOKEN" = Access token to Kubernetes API (4b*****d6)

# Using
After start service automatically searches all Pods in specified namespaces. Service provides WEB UI at 5000 port with Swagger interfaces for each Pod.
Service assumes what swgeer.json file should be hosted by /swagger/v1/swagger.json offset from 80 port in each Pod.

Target Services in Subs should give CORS allow so that the web application can load Swagger.json from a third-party domain
