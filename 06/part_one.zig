const std = @import("std");
const Io = std.Io;
const Reader = Io.Reader;
const Dir = Io.Dir;
const File = Io.File;
const Allocator = std.mem.Allocator;
const Tuple = std.meta.Tuple;
//const io = @import("io");

const PointArray = std.ArrayList(Point);
const ObstacleIdxArray = std.ArrayList(usize);

const Map = struct {
    obstacles: PointArray,
    row: std.ArrayList(ObstacleIdxArray),
    col: std.ArrayList(ObstacleIdxArray),

    pub fn init() Map {
        return .{
            .obstacles = PointArray.empty,
            .row = std.ArrayList(ObstacleIdxArray).empty,
            .col = std.ArrayList(ObstacleIdxArray).empty,
        };
    }

    pub fn deinit(self: *Map, gpa: Allocator) void {
        self.obstacles.deinit(gpa);
        for (0..self.row.items.len) |i| {
            self.row.items[i].deinit(gpa);
        }
        for (0..self.col.items.len) |i| {
            self.col.items[i].deinit(gpa);
        }
        self.row.deinit(gpa);
        self.col.deinit(gpa);
    }
};

const Point = struct {
    x: u32,
    y: u32,

    pub fn init(x: u32, y: u32) Point {
        return .{
            .x = x,
            .y = y,
        };
    }
};

const DirectionalPoint = struct {
    point: Point,
    direction: Direction,

    pub fn init(x: u32, y: u32, d: Direction) DirectionalPoint {
        return .{
            .point = Point.init(x, y),
            .direction = d
        };
    }
};

const Tile = enum {
    floor,
    obstacle,
    guard,
};

pub fn read_position(tile: u8) Tile {
    return switch(tile) {
        '.' => .floor,
        '#' => .obstacle,
        'v', '>', '<', '^' => .guard,
        else => unreachable,
    };
}

const Direction = enum {
    north,
    east,
    south,
    west,
};

pub fn read_direction(tile: u8) Direction {
    return switch(tile) {
        '^' => .north,
        '>' => .east,
        'v' => .south,
        '<' => .west,
        else => unreachable,
    };
}

pub fn load_map(alloc: Allocator, comptime T: type, reader: *T) !Tuple(&.{ Map, DirectionalPoint }) {
    var maybe_guard: ?DirectionalPoint = null;
    var map = Map.init();
    var row: u32 = 0;
    while (try reader.interface.takeDelimiter('\n')) |line| {
        try map.row.append(alloc, ObstacleIdxArray.empty);
        if (row == 0) {
            try map.col.ensureTotalCapacity(alloc, line.len+1);
            for (0..line.len) |_| {
                map.col.appendAssumeCapacity(ObstacleIdxArray.empty);
            }
        }
        for (line, 0..) |tile, col| {
            switch (read_position(tile)) {
                .obstacle => {
                    const position = Point.init(row, @intCast(col));
                    try map.obstacles.append(alloc, position);
                    const idx = map.obstacles.items.len;
                    try map.row.items[row].append(alloc, idx);
                    try map.col.items[col].append(alloc, idx);
                },
                .guard => maybe_guard = DirectionalPoint.init(row, @intCast(col), read_direction(tile)),
                else => {},
            }
        }
        row += 1;
    }
    if (maybe_guard) |guard| {
        return .{ map, guard };
    } else {
        return error.NoGuardInMap;
    }
}

pub fn main(init: std.process.Init) !u8 {
    const io = init.io;
    if (init.minimal.args.vector.len < 2) {
        std.debug.print("input file must be provided\n", .{});
        return 1;
    }
    const map_file_path: []const u8 = std.mem.span(init.minimal.args.vector[1]);

    const READ_ONLY: Dir.OpenFileOptions = .{ .mode = Dir.OpenFileOptions.Mode.read_only };
    const map_file = Dir.openFile(Dir.cwd(), io, map_file_path, READ_ONLY) catch |err| {
        std.debug.print("failed to open file {s}\n", .{map_file_path,});
        return err;
    };
    var buffer: [1024]u8 = undefined;
    const buf_slice: []u8 = buffer[0..1024];
    var reader = map_file.reader(io, buf_slice);
    var map: Map, const guard: DirectionalPoint = try load_map(init.gpa, @TypeOf(reader), &reader);
    defer map.deinit(init.gpa);


    std.debug.print("arg 1 {s}\n", .{map_file_path,});
    //std.debug.print("read {d} bytes\n", .{length,});
    //std.debug.print("{c}\n", .{content[4],});
    std.debug.print("guard location {any}\n", .{guard});
    std.debug.print("tile {any}\n", .{map.obstacles.items});
    std.debug.print("row indexes {any}\n", .{map.row.items});
    return 0;
}

test "test input" {
}
