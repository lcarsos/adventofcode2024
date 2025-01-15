const std = @import("std");
const inputReader = @import("inputReader.zig");
const parseList = inputReader.parseList;
const Allocator = std.mem.Allocator;
const ArrayList = std.ArrayList;

pub fn main() !void {
    var allocator = std.heap.GeneralPurposeAllocator(.{}){};
    const alloc = allocator.allocator();
    defer alloc.deinit();

    const stdin = std.io.getStdIn();
    var buf_reader = std.io.bufferedReader(stdin.reader());
    const again = buf_reader.reader().any();
    const parsed = try parseList(alloc, again);
    std.debug.print("{any}\n", .{parsed[0].items});

    std.mem.sort(i32, parsed[0].items, {}, std.sort.asc(i32));
    std.mem.sort(i32, parsed[1].items, {}, std.sort.asc(i32));
    std.debug.print("{any}\n", .{parsed[0].items});

    var diff = try ArrayList(i32).initCapacity(alloc, parsed[0].items.len);
    for (parsed[0].items, parsed[1].items, 0..) |x, y, i| {
        std.debug.print("{any} {any} {any}\n", .{ x, y, i });
        diff.append(@intCast(@abs(y - x))) catch unreachable;
    }
    var accum: i32 = 0;
    for (diff.items) |x| {
        accum += x;
    }
    const stdout = std.io.getStdOut();
    try std.fmt.format(stdout.writer(), "{}\n", .{accum});
}
