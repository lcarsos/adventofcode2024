const std = @import("std");
const Dir = std.Io.Dir;
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
        'v', '>', '<', '^' => .guard
    };
}

pub fn load_map(alloc: Allocator) .{PointArray, Point} {
    var obstacles: PointArray = std.ArrayList(Point).empty;
    obstacles.append(alloc, Point.init(0,0));
    return .{ obstacles, Point.init(0,0) };
}

pub fn main(init: std.process.Init) !void {
    const count = init.minimal.args.vector.len;
    const arg: []const u8 = std.mem.span(init.minimal.args.vector[1]);
    const io = init.io;

    var buffer: [1024]u8 = undefined;
    const buf_slice: []u8 = buffer[0..1024];

    const file_ref = Dir.openFile(Dir.cwd(), io, arg, .{ .mode = Dir.OpenFileOptions.Mode.read_only }) catch {
        std.debug.print("failed to open file {s}\n", .{arg,});
        return;
    };
    const length = try file_ref.readPositionalAll(io, buf_slice, 0);
    const content = buffer[0..length];


    std.debug.print("num args {d}\n", .{count,});
    std.debug.print("arg 1 {s}\n", .{arg,});
    std.debug.print("read {d} bytes\n", .{length,});
    std.debug.print("{c}\n", .{content[4],});
}

test "test input" {
}
