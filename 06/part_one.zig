const std = @import("std");
const Allocator = std.mem.Allocator;
const io = @import("io");

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
    const pwd = init.environ_map.get("CWD") orelse "N/A";
    const count = init.minimal.args.vector.len;
    const arg = init.minimal.args.vector[1];


    const file_ref = init.io.vtable.file



    std.debug.print("hello world {s}\n", .{pwd,});
    std.debug.print("num args {d}\n", .{count,});
    std.debug.print("arg 1 {s}\n", .{arg,});
}

test "test input" {
}
