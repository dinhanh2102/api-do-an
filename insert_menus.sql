INSERT INTO MenuSystem (Id, TitleDefault, Icon, Url, ParentId, IsDisable, [Index], IsDefaultMenu)
VALUES 
('M40', N'Quản lý nhân sự', N'user', N'', NULL, 0, 40, 1),
('M4001', N'Nhân viên', N'user', N'/nhan-vien/manage', 'M40', 0, 1, 1),
('M4002', N'Chức danh', N'category', N'/chuc-danh', 'M40', 0, 2, 1),
('M4003', N'Đơn vị', N'category', N'/don-vi', 'M40', 0, 3, 1),
('M4004', N'Phụ cấp', N'file', N'/phu-cap', 'M40', 0, 4, 1);
