# Microservices.Example

A simple setup of order, payment, and stock microservices has been created where they communicate with each other. Microservices do not have direct access to each other's databases, and data sharing is strictly controlled within limits set by the service sharing the data. MongoDB, SQL, and RabbitMQ are used for implementation. 

This communication example is designed solely for demonstrating inter-service communication in MCs. Actions that should not be performed in controllers are done for illustrative purposes or due to other exceptions in the rules being relaxed.
