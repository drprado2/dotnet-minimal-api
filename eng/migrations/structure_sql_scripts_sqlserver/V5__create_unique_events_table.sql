CREATE TABLE UniqueEvents
(
    EventId        uuid       NOT NULL,
    DateConsumed   TIMESTAMPTZ              NOT NULL,
    EventType      VARCHAR(255)           NOT NULL,
    ConsumerAction VARCHAR(255)           NOT NULL,
    InternalId     SERIAL NOT NULL,
    CONSTRAINT EventId_ConsumerAction_PK PRIMARY KEY (EventId, ConsumerAction)
);

CREATE INDEX idx_internal_event_type_id ON UniqueEvents (InternalId DESC, EventType);
