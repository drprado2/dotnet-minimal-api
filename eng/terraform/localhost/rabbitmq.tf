provider "rabbitmq" {
  endpoint = "http://rabbitmq:15672"
  username = "guest"
  password = "guest"
  insecure = true
}

resource "rabbitmq_vhost" "minimal_api_vhost" {
  name = "minimal_api"
}

resource "rabbitmq_permissions" "guest" {
  user  = "guest"
  vhost = rabbitmq_vhost.minimal_api_vhost.name

  permissions {
    configure = ".*"
    write     = ".*"
    read      = ".*"
  }
}

resource "rabbitmq_exchange" "entity_created" {
  name  = "entity_created"
  vhost = rabbitmq_permissions.guest.vhost

  settings {
    type        = "fanout"
    durable     = true
    auto_delete = false
  }
}

resource "rabbitmq_exchange" "entity_updated" {
  name  = "entity_updated"
  vhost = rabbitmq_permissions.guest.vhost

  settings {
    type        = "fanout"
    durable     = true
    auto_delete = false
  }
}

resource "rabbitmq_queue" "minimal_api_worker_entity_created" {
  name  = "entity_created"
  vhost = rabbitmq_permissions.guest.vhost

  settings {
    durable     = true
    auto_delete = false
    arguments = {
      "x-queue-type" : "quorum",
      "x-delivery-limit": 4
    }
  }
}

resource "rabbitmq_queue" "minimal_api_worker_entity_updated" {
  name  = "entity_updated"
  vhost = rabbitmq_permissions.guest.vhost

  settings {
    durable     = true
    auto_delete = false
    arguments = {
      "x-queue-type" : "quorum",
      "x-delivery-limit": 4
    }
  }
}

resource "rabbitmq_binding" "minimal_api_worker_entity_created" {
  source           = rabbitmq_exchange.entity_created.name
  vhost            = rabbitmq_vhost.minimal_api_vhost.name
  destination      = rabbitmq_queue.minimal_api_worker_entity_created.name
  destination_type = "queue"
  routing_key      = "#"
}

resource "rabbitmq_binding" "minimal_api_worker_entity_updated" {
  source           = rabbitmq_exchange.entity_updated.name
  vhost            = rabbitmq_vhost.minimal_api_vhost.name
  destination      = rabbitmq_queue.minimal_api_worker_entity_updated.name
  destination_type = "queue"
  routing_key      = "#"
}

resource "rabbitmq_policy" "max_retry_policy" {
  name  = "max_retry_policy"
  vhost = rabbitmq_vhost.minimal_api_vhost.name

  policy {
    pattern  = ".*"
    priority = 0
    apply_to = "queues"

    definition = {
      delivery-limit = 4
    }
  }
}