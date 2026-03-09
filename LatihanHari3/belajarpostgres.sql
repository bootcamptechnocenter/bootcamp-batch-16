--create table master bank
create table MST_BRANDS(
	ID SERIAL primary key,
	CODE VARCHAR(10) not null,
	NAME VARCHAR(100) not null,
	IS_ACTIVE BOOLEAN not null default true,
	CREATED_AT timestamp not null default NOW(),
	CREATED_BY VARCHAR(100),
	UPDATED_AT timestamp,
	UPDATED_BY VARCHAR(100),
	DELETED_AT timestamp,
	DELETED_BY VARCHAR(100)
);


insert
	into
	mst_brands(code, name, created_by)
values('HON', 'Honda', 'Admin');


insert
	into
	mst_brands(code, name, created_by)
values('TOY', 'TOYOTA', 'Admin'),
('SUZ',
'SUZUKI',
'Admin'),
('DAI',
'DAIHATSU',
'Admin');


select * from mst_brands;

select
	ID,
	CODE,
	NAME
from
	mst_brands mb
where
	is_active = true
	and mb.deleted_at is null
order by
	NAME asc;

--SEARCH
select
	*
from
	mst_brands mb
where
	mb.deleted_at is null
	and NAME ilike '%toy%';

select
	count(*)
from
	mst_brands
where
	deleted_at is null;

update
	mst_brands mb
set
	name = 'HONDA',
	updated_at = now(),
	updated_by = 'Admin'
where
	code = 'HON'

update
	mst_brands mb
set
	deleted_at = now(),
	deleted_by = 'Admin'
where
	code = 'TOY';



create table mst_types(
	id serial primary key,
	brand_id integer not null references mst_brands(id),
	code varchar (10) not null,
	name varchar(100) not null,
	is_active boolean not null default true,
	CREATED_AT timestamp not null default NOW(),
	CREATED_BY VARCHAR(100),
	UPDATED_AT timestamp,
	UPDATED_BY VARCHAR(100),
	DELETED_AT timestamp,
	DELETED_BY VARCHAR(100)
)


insert
	into
	mst_types(brand_id, code, name, created_by)
values(1, 'JAZZ', 'Honda Jazz', 'Admin'),
(1,'CRV','Honda Crv','Admin'),
(2,'MX','Toyota Mark X','Admin');

select * from mst_types;

select
	t.id as type_id,
	t.code as type_code,
	t.name as type_name,
	b.name as brand_name
from
	mst_types t
join mst_brands b on
	t.brand_id = b.id
where
	t.deleted_at is null
	and b.deleted_at is null
order by
	b.name,
	t.name;


create or replace
procedure insert_brand(
in p_code varchar,
in p_name varchar,
in p_created_by varchar,
out out_stat boolean,
out out_mess text
)
language plpgsql
as $$
begin
	if exists
    (
    select 1 from mst_brands where
        code = p_code
        and deleted_at is null
        ) then
        out_stat := false;
        out_mess := 'Kode sudah dipakai: ' || p_code;
    end if;
insert
	into
	mst_brands (code,
	name,
	created_by,
	created_at)
values (p_code,
p_name,
p_created_by,
now());
out_stat := true;
out_mess := 'Data berhasil ditambahkan';
end;
$$;

call insert_brand('MZD', 'MAZDA', 'Admin', null, null);




create or replace
procedure delete_brand(
in p_id int,
out out_stat boolean,
out out_mess text
)
language plpgsql
as $$
begin
	if not exists
    (
    select 1 from mst_brands where
        id = p_id
        and deleted_at is null
        ) then
        out_stat := false;
        out_mess := 'id tidak ditemukan atau sudah di-delete: ' || p_id;
        return;
    end if;
update
	mst_brands mb
set
	deleted_at = now(),
	deleted_by = 'Admin'
where
	id = p_id;
out_stat := true;
out_mess := 'Data berhasil di-delete';
end;
$$;

select * from mst_brands

call delete_brand(3, null, null);


create or replace view v_types_with_brands as
select
	mt.id as type_id,
	mt.name as type_name,
	mt.code as type_code,
	mb.id as brand_id,
	mb.name as brand_name
from mst_types mt
join mst_brands mb on mb.id = mt.brand_id
where mt.deleted_at is null and mb.deleted_at is null;

-- select view
select * from v_types_with_brands;


create table mst_models(
	id serial primary key,
	type_id integer not null references mst_types(id),
	code varchar (10) not null,
	name varchar(100) not null,
	year integer,
	is_active boolean not null default true,
	CREATED_AT timestamp not null default NOW(),
	CREATED_BY VARCHAR(100),
	UPDATED_AT timestamp,
	UPDATED_BY VARCHAR(100),
	DELETED_AT timestamp,
	DELETED_BY VARCHAR(100)
)

select * from mst_types

insert
	into
	mst_models(type_id, code, name, year, created_by)
values('1', 'GD3', 'Generasi 1', 2007, 'Admin'),
('1', 'GE3', 'Generasi 2', 2013, 'Admin'),
('1', 'GK5', 'Generasi 3', 2021, 'Admin');


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



create or replace
procedure insert_type(
in p_brand_id int,
in p_code varchar,
in p_name varchar,
in p_created_by varchar,
out out_stat boolean,
out out_mess text
)
language plpgsql
as $$
begin
	if not exists
    (
    select 1 from mst_brands where
        id = p_brand_id
        and deleted_at is null
        ) then
        out_stat := false;
        out_mess := 'Brand ID tidak ditemukan, brand_id: ' || p_brand_id;
        return;
    end if;
if exists
(
select 1 from mst_types where
	brand_id = p_brand_id and code = p_code
	and deleted_at is null and is_active = true
	) then
	out_stat := false;
    out_mess := 'Sudah ada type dengan brand id: ' || p_brand_id || ' dan code:' || p_code;
    return;
end if;
insert
	into
	mst_types(brand_id, code, name, created_by)
values(p_brand_id, p_code, p_name, 'Admin');
out_stat := true;
out_mess := 'Type berhasil ditambahkan';
end;
$$;


call insert_type(2, 'UNIQ', 'HARUSNYA BISA', 'Admin', null, null);


select * from mst_types

select * from mst_brands