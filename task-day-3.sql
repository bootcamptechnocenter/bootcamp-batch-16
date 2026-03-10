-- TASK 1
create table mst_models (
	id serial primary key ,
	type_id integer not null references mst_types(id),
	code varchar(10) not null,
	name varchar(100) not null,
	year integer,
	create_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
);

select * from mst_models

insert into mst_models (TYPE_ID, code, name, year, created_by)
values (1, 'JCV', 'JAZZ CVT', 2022, 'Admin' ),
(1, 'RS', 'JAZZ RS', 2020, 'Admin' ),
(2, 'CPR', 'CRV PRESTIGE', 2021, 'Admin' );

select
	mb."name" ,
	mt."name" ,
	mm."name"
from
	mst_brands mb ,
	mst_models mm ,
	mst_types mt
where
	mb.deleted_at is null
	and mm.deleted_at is null
	and mt.deleted_at is null
	and mb.id = mt.brand_id
	and mt.id = mm.type_id ;


-- TASK 2
create or replace procedure insert_types (
	in p_brand_id integer,
	in p_code varchar,
	in p_name varchar,
	in p_is_active boolean,
	in p_created_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
begin
	if exists (
		select 1 from mst_brands
		where id = p_brand_id and deleted_at is null
	) then
		if not exists  (
			select 1 from mst_types
			where brand_id = p_brand_id and deleted_at is null)
		and not exists (
			select 1 from mst_types
			where code = p_code and deleted_at is null)
			then
				insert into mst_types (brand_id, code, name, is_active, created_by, create_at)
					values (p_brand_id, p_code, p_name, p_is_active, p_created_by, now());
				out_stat := true;
				out_mess := 'Data berhasil ditambahkan';
		else 
			out_stat := false;
			out_mess := 'Kode atau brand id sudah dipakai ';
			return;
		end if;
	else
		out_stat := false;
		out_mess := 'brand id tidak ada di mst_brands';
		return;
	end if;
end;
$$