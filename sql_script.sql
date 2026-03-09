-- create table mst_brands
create table mst_brands(
	id serial primary key,
	code varchar(10) not null,
	name varchar(100) not null,
	is_active boolean not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
);

-- insert data
insert into training_dotnet.public.mst_brands(code, name, created_by) values ("HON", "Honda", "Admin");

-- insert multiple data
insert into training_dotnet.public.mst_brands(code, name, created_by)
values
	("TOY", "Toyota", "Admin"),
	("SUZ", "Suzuki", "Admin"),
	("DAI", "Daihatsu", "Admin");

-- select table
select * from training_dotnet.public.mst_brands;

-- select deleted_at = null
select * from training_dotnet.public.mst_brands where deleted_at is null;

-- select + filter + sort
select id, name, is_active from training_dotnet.public.mst_brands mb where
mb.deleted_at is null and mb.is_active = true order by name asc;

-- select case-insensitive
select * from training_dotnet.public.mst_brands mb where mb.deleted_at is null and name ilike '%toy%';

-- count
select count(*) from training_dotnet.public.mst_brands mb where mb.deleted_at is null;

-- update
update training_dotnet.public.mst_brands mb
set name = 'Honda Motor',
	updated_at = now(),
	updated_by = 'Admin'
where id = 1
and mb.deleted_at is null;

-- soft delete
update training_dotnet.public.mst_brands mb
set deleted_at = now(),
	deleted_by = 'Admin'
where id = 1
and mb.deleted_at is null;

-- restore deletion
update training_dotnet.public.mst_brands mb
set deleted_at = null,
	deleted_by = null
where id = 1
and mb.deleted_at is not null;

-- create table mst_type
create table training_dotnet.public.mst_types(
	id serial primary key,
	brand_id integer not null references training_dotnet.public.mst_brands(id),
	code varchar(10) not null,
	name varchar(100) not null,
	is_active boolean not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
);

-- insert data
insert into training_dotnet.public.mst_types(brand_id, code, name, created_by) values (1, 'JAZZ', 'Honda Jazz', 'Admin');

-- insert multiple data
insert into training_dotnet.public.mst_types(brand_id, code, name, created_by)
values
	(1, 'CRV', 'Honda CRV', 'Admin'),
	(2, 'MX', 'Toyota Mark X', 'Admin');


-- select table
select * from training_dotnet.public.mst_types;

-- select + join
select
	mt.id as type_id,
	mt.code as type_code,
	mt.name as type_name,
	mb.name as brand_name
from training_dotnet.public.mst_types mt
join training_dotnet.public.mst_brands mb on mb.id = mt.brand_id
where mt.deleted_at is null and mb.deleted_at is null
order by mb.name, mt.name;