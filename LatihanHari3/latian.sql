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
)


-- INSERT BRAND
insert into mst_brands(code, name, created_by)
values ('HON', 'Honda', 'Admin')


-- BULK INSERT
insert into mst_brands(code, name, created_by)
values 
	('TOY', 'Toyota', 'Admin'),
	('SUZ', 'Suzuki', 'Admin'),
	('DAI', 'Daihatsu', 'Admin');


-- SELECT YANG BELUM DIHAPUS
select * from mst_brands mb where mb.deleted_at is null;


-- SELECT + FILTER + SORT
select id, name, is_active
from mst_brands where
deleted_at is null
and is_active = true
order by name asc;

-- PENCARIAN CASE INSENSITIVE
select * from mst_brands
where deleted_at is null
and name ilike '%oyo%';


-- COUNT
select count(*) from mst_brands where deleted_at is null;


-- UPDATE
update mst_brands set
	name = 'Honda Motor Indonesia',
	updated_at = now(),
	updated_by = 'Admin'
where id = 1
and deleted_at is null;


-- SOFT DELETE
update mst_brands set
	deleted_at = now(),
	deleted_by = 'Admin',
	is_active = false 
where id = 2
and deleted_at is null;
	

-- CREATE TABLE MST_TYPES
create table mst_types(
	id serial primary key,
	brand_id integer not null references mst_brands(id),
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


-- INSERT DATA MST_TYPES
insert into mst_types(brand_id, code, name, created_by)
values
	(1, 'JAZZ', 'Honda Jazz', 'Admin'),
	(1, 'CRV', 'Honda Crv', 'Admin'),
	(2, 'MX', 'Toyota Mark X', 'Admin');


-- JOIN
select 
	t.id as type_id,
	t.code as type_code,
	t.name as type_name, 
	b.name as brand_namealter 
from mst_types t
join mst_brands b
on b.id = t.brand_id 
where t.deleted_at is null
and b.deleted_at  is null
order by b.name, t.name;


-- PROCEDURE
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
--	cek duplikat
	if exists(
		select 1 from mst_brands
		where code = p_code and deleted_at is null
	) then
		out_stat := false;
		out_mess := 'Kode sudah dipakai '||p_code;
		return;
	end if;

	insert into mst_brands(code, name, created_by, created_at)
	values (p_code, p_name, p_created_by, now());
	
	out_stat := true;
	out_mess := 'Data berhasil ditambahkan';
end;
$$;

call insert_brand('MZD', 'Mazda', 'Admin', null, null);


-- PROCEDURE TO SOFT DELETE
create or replace procedure soft_delete_brand(
	in p_brand_id int,
	out out_stat boolean,
	out out_message text
)
language plpgsql
as $$
begin
	if exists(
		select 1 from mst_brands
		where id = p_brand_id
		and deleted_at is null
	) then
		update mst_brands set
			deleted_at = now(),
			deleted_by = 'Admin',
			is_active = false 
		where id = p_brand_id;


		out_stat := true;
		out_message := 'Brand dengan id : ' ||p_brand_id || ' berhasil dihapus.';
		return;
	end if;
		
	out_stat := false;
	out_message := 'Brand tidak ditemukan';
end;
$$;

call soft_delete_brand(3, null, null);


-- CREATE VIEW DENGAN JOIN
create or replace view v_types_with_brand as
select
	t.id		as type_id,
	t.name 		as type_name,
	t.code		as type_code,
	b.id		as brand_id,
	b.name		as brand_name
from mst_types t
join mst_brands b on t.brand_id = b.id
where t.deleted_at is null
and b.deleted_at is null;

select * from v_types_with_brand;


-- LATIHAN
create table mst_models(
	id serial primary key,
	type_id integer not null references mst_types(id),
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
)

insert into mst_models(type_id, code, name, year)
values
	(1, 'HBR', 'Hybrid', 2022),
	(1, 'ICE', 'Internal Combustion Engine', 2022),
	(1, 'EV', 'Electric Vehicle', 2022);

select b.name as brand_name, t.name as type_name, m.name as model_name, m.code, m.year
	from mst_models m join mst_types t on m.type_id = t.id
	join mst_brands b on t.brand_id = b.id
	where b.is_active = true
	and m.is_active = true
	and t.is_active = true
	order by b.name, t.name, m.name;

create or replace procedure insert_type(
    in p_brand_id       int,
    in p_code           varchar(10),
    in p_name           varchar(100),
    in p_created_by     varchar(100),
    out out_stat        boolean,
    out out_message     text
)
language plpgsql
as $$
declare
    v_exists int;
begin
    select 1 into v_exists from mst_brands 
    where id = p_brand_id and is_active = true;

    if v_exists is null then
        out_stat := false;
        out_message := 'Brand tidak ditemukan atau tidak aktif.';
        return;
    end if;


    v_exists := null;
    select 1 into v_exists from mst_types 
    where code = p_code and brand_id = p_brand_id;

    if v_exists is not null then
        out_stat := false;
        out_message := 'Kombinasi brand dan type yang diinputkan sudah tersedia.';
        return;
    end if;

    insert into mst_types (brand_id, code, "name", created_by, created_at)
    values (p_brand_id, p_code, p_name, p_created_by, now());

    out_stat := true;
    out_message := 'Berhasil menambahkan type.';
end;
$$;


call insert_type(10, 'JAZZ', 'Honda Jazz', 'Admin', null, null);
