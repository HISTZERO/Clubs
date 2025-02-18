Part 2 - The Design Challenge

1\) Could you please describe your ideal strategy to handle interservice
communications in a microservice environment, especially when hosted in
the cloud?

In cloud, my ideal strategy to handle interservice communications in a
microservice environment can be use in:

\- Use Azure Service Bus to decouple services, ensuring reliable and
scalable comunication.

\- Use Azure API Management as a gateway to expose APIs and route
request.

\- Use Azure Monitor for distributed tracing and logging, so you can
monitor interservice communications and troubleshoot issues.

2\) What could be the consequences of not adequately handling
interservice

communications in a microservice environment?

\- Cascading Failures : One service fails can cause multiple dependent
services fails

\- Bottlenecks and Increased Latency

\- Reduced Scalability and Resilience

\- Complex debugging
