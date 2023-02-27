CREATE TABLE [task]
(
    id                  int IDENTITY (1,1),
    [description]       varchar(255)                       NOT NULL,
    [date]              datetime                       NOT NULL,
    [status]            bit              DEFAULT 0         NOT NULL,
    created_at          datetime         DEFAULT GETDATE() NOT NULL,
    updated_at          datetime         DEFAULT null      NULL,
    CONSTRAINT ["pk_task"] PRIMARY KEY NONCLUSTERED (id)
)
GO