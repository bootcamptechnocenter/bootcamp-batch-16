create table  mst_brands(
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
)

--insert brand
insert into mst_brands(code, name, created_by) values('HON', 'Honda', 'Admin')

insert into mst_brands(code, name, created_by)
values('TOY', 'Toyota', 'Admin'), ('SUZ', 'Suzuki', 'Admin'), ('DAI', 'Daihatsu', 'Admin')

select * from mst_brands where deleted_at is null;

select id, name, is_active
from mst_brands mb where
deleted_at is null 
and is_active = true
order by name asc

--pencarian case-insensitive
select * from mst_brands
where deleted_at is null 
and name ilike '%toy%'

--count 
select count(*) from mst_brands where deleted_at is null;

--update
update mst_brands
set name = 'Honda Motor Indonesia',
updated_at = now(),
updated_by = 'Admin'
where id = 1
and deleted_at is null


--delete (soft delete)
update mst_brands 
set deleted_at = now(),
deleted_by = 'Admin',
is_active = false
where id = '2';


--create table mst_type
create table mst_types(
	id serial primary key,
	brand_id integer not null references mst_brands(id),
	code varchar (10) not null,
	name varchar (100) not null,
	is_active boolean not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
)

--insert data mst_types
insert into mst_types(brand_id, code, name, created_by)
values
(1, 'JAZZ', 'Honda Jazz', 'Admin'),
(2, 'CRV', 'Honda Crv', 'Admin'),
(3, 'MX', 'Toyota Mark X', 'Admin');

select 
t.id as type_id,
t.code as type_code,
t.name as type_name,
b.name as brand_name
from mst_types t
join mst_brands b 
on t.brand_id = b.id
where t.deleted_at is null and b.deleted_at is null order by b.name, t.name

--create PRC 
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
	--cek duplikat
	if exists(
		select 1 from mst_brands
		where code = p_code and deleted_at is null
	) then 
		out_stat := false;
		out_mess := 'Kode sudah dipakai: ' || p_code
		return;
	end if;
	
	insert into mst_brands (code, name, created_by, created_at)
	values (p_code, p_name, p_created_by, now());
	
	out_stat := true;
	out_mess := 'Data berhasil ditambahkan';
end
$$

call insert_brand('MZD', 'Mazda', 'Admin', null, null)

--create PRC 
create or replace procedure delete_brand(
	in p_code varchar,
	in p_deleted_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
begin
	if exists(
		select 1 from mst_brands
		where code = p_code and deleted_at is not null
	) then 
		out_stat := false;
		out_mess := 'Kode sudah dihapus' || p_code;
		return;
	end if;	
	update mst_brands 
	set deleted_at = now(),
		deleted_by = 'Admin',
		is_active = false
	where code = p_code;
	
	
	out_stat := true;
	out_mess := 'Data berhasil dihapus';
end
$$

select * from mst_brands

call delete_brand('MZD', 'Admin', null, null)

--create view dengan join
create or replace view v_types_with_brand as 
select 
	t.id as type_id,
	t.name as type_name,
	t.code as type_code,
	b.id as brand_id,
	b.name as brand_name
from mst_types t
join mst_brands b on t.brand_id = b.id
where t.deleted_at is null
and b.deleted_at is null;

select * from v_types_with_brand;

create table mst_models(
	id serial primary key,
	type_id integer not null references mst_types(id),
	code varchar (10) not null,
	name varchar (100) not null,
	year integer,
	is_active boolean not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
)

--create view dengan join
select
	m.id as model_id,
	m.code as model_code,
	m.name as model_name,
	t.name as type_name,
	b.name as brand_name
from mst_types t
join mst_brands b 
on t.brand_id = b.id
join mst_models m
on m.type_id = t.id
where t.deleted_at is null
and b.deleted_at is null;


select * from mst_models

select * from mst_types

insert into mst_models(type_id, code, name, year, created_by)
values
(1, 'RS', 'All new RS', 2020, 'Admin'),
(1, 'AT', 'Automatic standard', 2020,'Admin'),
(1, 'MT', 'Manual standard', 2020,'Admin');

create or replace procedure insert_types(
	in p_brand_id integer,
	in p_code varchar,
	in p_name varchar,
	in p_created_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
begin
	--cek duplikat
	
	if not exists(
		select 1 from mst_brands
		where id = p_brand_id and deleted_at is null
	) then 
		out_stat := false;
		out_mess := 'Brand tidak valid: ' || p_brand_id;
		return;
	end if;


	if exists(
		select 1 from mst_types
		where code = p_code and deleted_at is null
	) then 
		out_stat := false;
		out_mess := 'Kode type sudah dipakai: ' || p_code;
		return;
	end if;

	
	insert into mst_types(brand_id, code, name, created_by, created_at)
	values (p_brand_id, p_code, p_name, p_created_by, now());
	
	out_stat := true;
	out_mess := 'Data berhasil ditambahkan';
end
$$

select * from mst_types

select * from mst_brands

call insert_types(3, 'JIMNY', 'Jimny', 'Admin', null, null)

call insert_types(6, 'JIMNY', 'Jimny', 'Admin', null, null)





