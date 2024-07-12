# MassTransit.RabbitSingleActiveConsumer

The use case for this is protecting access to a limited resource.

Rabbit's SingleAccessConsumer approach allows you to proceed without having to figure out a DotNet distributed Rate Limiter.

Each instance can have it's own internal rate limiter that it can freely abide by, knowing that no other consumer is also doing work.

Of course, a Distributed Rate Limiter would be a better solution...

(Feel free to point me in that direction...)