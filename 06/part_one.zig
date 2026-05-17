const std = @import("std");
const Io = std.Io;
const Dir = Io.Dir;
const File = Io.File;
const Allocator = std.mem.Allocator;
const Tuple = std.meta.Tuple;
//const io = @import("io");

const PointArray = std.ArrayList(Point);

const Map = struct {
    row: std.ArrayList(PointArray),
    col: std.ArrayList(PointArray),

    pub fn init() Map {
        return .{
            .row = std.ArrayList(PointArray).empty,
            .col = std.ArrayList(PointArray).empty,
        };
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

pub fn load_map(alloc: Allocator, io: Io, file: File) !Tuple(&.{ Map, DirectionalPoint }) {
    var buffer: [1024]u8 = undefined;
    const buf_slice: []u8 = buffer[0..1024];
    var reader = file.reader(io, buf_slice);

    var maybe_guard: ?DirectionalPoint = null;
    var map = Map.init();
    var row: u32 = 0;
    while (try reader.interface.takeDelimiter('\n')) |line| {
        for (line, 0..) |col, i| {
            switch (read_position(col)) {
                .obstacle => {
                    try map.row.items[row].append(alloc, Point.init(row, @intCast(i)));
                },
                .guard => maybe_guard = DirectionalPoint.init(row, @intCast(i), read_direction(col)),
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
    var map: Map, const guard: DirectionalPoint = try load_map(init.gpa, io, map_file);
    defer map.row.deinit(init.gpa);
    defer map.col.deinit(init.gpa);


    std.debug.print("arg 1 {s}\n", .{map_file_path,});
    //std.debug.print("read {d} bytes\n", .{length,});
    //std.debug.print("{c}\n", .{content[4],});
    std.debug.print("guard location {any}\n", .{guard});
    std.debug.print("tile {any}\n", .{map.items[0]});
    return 0;
}

test "test input" {
}
