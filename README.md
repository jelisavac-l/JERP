<div align="center">

<h1>🐾 Jezdimirov Reverse Proxy & Load Balancer</h1>
<p>
  Programmable reverse proxy and load balancer built with ASP.NET Core. Acting as as a gateway for distributed systems.
  It retrieves and distributes incoming traffic across multiple backend clusters using advanced routing and health-checking mechanisms.
</p>
<img width="1280" height="376" alt="JERP logo" src="https://github.com/user-attachments/assets/2f410899-7018-46b1-8afd-dd9afd866197" />
</div>

## 🎓 Academic context

This project is being developed as part of the "Advanced .NET Technologies" course at University of Belgrade, Faculty of organisational sciences. 
It demonstrates the implementation of asynchronous request transformation, and dynamic cluster management within the .NET platform.

## 🦴 Features

### ⚖️ Dynamic Load Balancing
Supports `FirstAvailable`, `RoundRobin`, `WeightedRoundRobin`, `IpHashing` and `Random` policies to ensure even distribution across backend nodes.
### ❤️ Active Health Checks
Automatically monitors the status of destination servers and pulls unhealthy nodes out of the rotation. 
### 🚀 Request Transformation
Custom middleware to modify headers, paths, and query parameters on the fly. 
### 👁️ Distributed Observability
Integrated logging and telemetry to track request flow from the edge to the microservice.

## Try it for yourself
Spin up 3 simple HTTP apps in docker and create a `config.json`:
```json
{
  "serverName": "JERP Example Config",
  "services": [
    {
      "Name": "users-service",
      "path": "/api/users",
      "loadBalancingPolicy": "fa",
      "stripPrefix": true,
      "retries": 1,
      "timeout": 5,
      "instances": [
        {
          "url": "http://localhost:8081",
          "healthCheckPath": "/health"
        }
      ],
      "addHeaders": {
        "X-Some-Header": "Hello",
        "X-Some-Other-Header": "World"
      }
    },
    {
      "Name": "product-service",
      "path": "/api/products",
      "loadBalancingPolicy": "rr",
      "stripPrefix": true,
      "retries": 3,
      "timeout": 30,
      "instances": [
        {
          "url": "http://localhost:8082",
          "healthCheckPath": "/health"
        },
        {
          "url": "http://localhost:8083",
          "healthCheckPath": "/health"
        }
      ]
    }
  ]
}
```