const std = @import("std");
const Io = std.Io;
const Dir = Io.Dir;
const File = Io.File;
const Allocator = std.mem.Allocator;
//const io = @import("io");

const PointArray = std.ArrayList(Point);

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

const LoadMapTuple = std.meta.Tuple(&.{ PointArray, Point });

pub fn load_map(alloc: Allocator, io: Io, file: File) !LoadMapTuple {
    var buffer: [1024]u8 = undefined;
    const buf_slice: []u8 = buffer[0..1024];
    var reader = file.reader(io, buf_slice);

    var obstacles: PointArray = std.ArrayList(Point).empty;
    const line = try reader.interface.takeDelimiter('\n') orelse "";
    for (line, 0..) |col, i| {
        if (read_position(col) == Tile.obstacle) {
            try obstacles.append(alloc, Point.init(0, @intCast(i)));
        }

    }
    //obstacles.append(alloc, Point.init(0,0));
    return .{ obstacles, Point.init(0,0) };
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
    const map_res = try load_map(init.gpa, io, map_file);
    //const map: PointArray = map_res[0];
    const guard = map_res[1];
    //defer init.gpa.free(map.items);
    //const length = try map_file.readPositionalAll(io, buf_slice, 0);
    //const content = buffer[0..length];


    std.debug.print("arg 1 {s}\n", .{map_file_path,});
    //std.debug.print("read {d} bytes\n", .{length,});
    //std.debug.print("{c}\n", .{content[4],});
    std.debug.print("guard location {any}\n", .{guard});
    return 0;
}

test "test input" {
}
