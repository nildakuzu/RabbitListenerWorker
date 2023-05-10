# RabbitListenerWorker

RabbitListenerWorker consumes some urls from the RabbitMQ. Urls are receveied by eventbus(in memory). After consuming urls from RabbitMQ, they are loged with json format and send to Redis Cache. Project and third part applications(RabbitMQ, Redis) are hosted in Docker with docker-compose. Project's framework is net6.0 and developed with clean architecture.

Structure of rabbitlistener worker is showing at the below picture.
![Screenshot 2023-05-10 161615](https://github.com/nildakuzu/RabbitListenerWorker/assets/8994712/29fb10b6-0f3a-44af-bf2c-b3f64de112dc)

## RabbitMQ Message Payload Example

When you run project, a queue is occurred which name is 'urls'. You can push message to this queue like at the below payload.

- {
	"Url" : "https://www.youtube.com"
}

- {
	"Url" : "https://www.google.com"
}

## Hosted External Application Links

After run, external applications are hosted in the docker with below links.

- RaabitMQ: http://localhost:15672/#/queues
- Redis: http://localhost:8001/
