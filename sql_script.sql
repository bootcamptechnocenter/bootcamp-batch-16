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

-- create prc with duplicate id validation
create or replace procedure insert_brand(
	in p_code varchar,
	in p_name varchar,
	in p_created_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
begin
	-- check duplicate
	if exists(
		select 1 from training_dotnet.public.mst_brands mb
		where mb.code = p_code and mb.deleted_at is null
	) then
		out_stat := false;
		out_mess := 'Code already used: ' || p_code;
		return;
	end if;

	-- insert data
	insert into training_dotnet.public.mst_brands(code, name, created_by, created_at)
	values (p_code, p_name, p_created_by, now());

	out_stat := true;
	out_mess := 'Brand inserted successfully';
end;
$$;

-- call prc
call insert_brand('MZ', 'Mazda', 'Admin', null, null);
call insert_brand('HYN', 'Hyundai', 'Admin', null, null);

-- create prc for soft delete with check if id exists
create or replace procedure delete_brand(
	in p_id integer,
	in p_deleted_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
declare
	v_exist boolean;
begin
	-- check if id exists
	select exists(
		select 1 from training_dotnet.public.mst_brands mb
		where mb.id = p_id and mb.deleted_at is null
	) into v_exist;

	if not v_exist then
		out_stat := false;
		out_mess := 'Brand not found with id: ' || p_id;
		return;
	end if;

	-- soft delete
	update training_dotnet.public.mst_brands mb
	set deleted_at = now(),
		deleted_by = p_deleted_by
	where mb.id = p_id and mb.deleted_at is null;

	out_stat := true;
	out_mess := 'Brand deleted successfully';
end;
$$;

-- call prc
call delete_brand(1, 'Admin', null, null);
call delete_brand(10, 'Admin', null, null);

-- create view with join
create or replace view v_types_with_brands as
select
	mt.id as type_id,
	mt.name as type_name,
	mt.code as type_code,
	mb.id as brand_id,
	mb.name as brand_name,
from training_dotnet.public.mst_types mt
join training_dotnet.public.mst_brands mb on mb.id = mt.brand_id
where mt.deleted_at is null and mb.deleted_at is null;

-- select view
select * from v_types_with_brands;

-- exercise
-- create table mst_models with columns id, type_id (FK to mst_types), code, name, year (integer), created_at, created_by, updated_at, updated_by, deleted_at, deleted_by
create table training_dotnet.public.mst_models(
	id serial primary key,
	type_id integer not null references training_dotnet.public.mst_types(id),
	code varchar(10) not null,
	name varchar(100) not null,
	year integer not null,
	is_active boolean not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
);

-- create 3 models
insert into training_dotnet.public.mst_models(type_id, code, name, year, created_by) values
(1, 'JAZZ2020', 'Honda Jazz 2020', 2020, 'Admin'),
(2, 'CRV2021', 'Honda CRV 2021', 2021, 'Admin'),
(3, 'MX2020', 'Toyota Mark X 2020', 2020, 'Admin');

-- select with join to mst_types and mst_brands
select
	mm.id as model_id,
	mm.code as model_code,
	mm.name as model_name,
	mm.year as model_year,
	mt.name as type_name,
	mb.name as brand_name
from training_dotnet.public.mst_models mm
join training_dotnet.public.mst_types mt on mt.id = mm.type_id
join training_dotnet.public.mst_brands mb on mb.id = mt.brand_id
where mm.deleted_at is null and mt.deleted_at is null and mb.deleted_at is null
order by mb.name, mt.name, mm.year desc;

-- create prc to insert type with brand id exist validation and code + brand id duplicate validation
create or replace procedure insert_type(
	in p_brand_id integer,
	in p_code varchar,
	in p_name varchar,
	in p_created_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
declare
	v_exist boolean;
	v_is_duplicate boolean;
begin
	-- check if brand id exists
	select exists(
		select 1 from training_dotnet.public.mst_brands mb
		where mb.id = p_brand_id and mb.deleted_at is null
	) into v_exist;

	if not v_exist then
		out_stat := false;
		out_mess := 'Brand not found with id: ' || p_brand_id;
		return;
	end if;

	-- check for duplicate code and brand id
	select exists(
		select 1 from training_dotnet.public.mst_types mt
		where mt.brand_id = p_brand_id and mt.code = p_code and mt.deleted_at is null
	) into v_is_duplicate;

	if v_is_duplicate then
		out_stat := false;
		out_mess := 'Type with code: ' || p_code || ' already exists for brand id: ' || p_brand_id;
		return;
	end if;

	-- insert data
	insert into training_dotnet.public.mst_types(brand_id, code, name, created_by, created_at)
	values (p_brand_id, p_code, p_name, p_created_by, now());

	out_stat := true;
	out_mess := 'Type inserted successfully';
end;
$$;

-- call prc
call insert_type(1, 'CRV', 'Honda CRV', 'Admin', null, null);
call insert_type(1, 'CIVIC', 'Honda Civic', 'Admin', null, null);
call insert_type(10, 'COROLLA', 'Toyota Corolla', 'Admin', null, null);
