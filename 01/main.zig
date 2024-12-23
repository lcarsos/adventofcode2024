const std = @import("std");
const Allocator = std.mem.Allocator;
const ArrayList = std.ArrayList;

fn parseList(alloc: Allocator, input: std.io.AnyReader) ![2]ArrayList(i32) {
    var first = ArrayList(i32).init(alloc);
    var second = ArrayList(i32).init(alloc);

    var line_buffer: [16:0]u8 = undefined;
    while (input.readUntilDelimiter(&line_buffer, '\n')) |line| {
        var strs = std.mem.splitSequence(u8, line, "   ");
        const match: i32 = std.fmt.parseInt(i32, strs.next().?, 10) catch return .{ first, second };
        std.debug.print("look: {any}\n", .{match});
        first.append(match) catch unreachable;
        second.append(std.fmt.parseInt(i32, strs.next().?, 10) catch unreachable) catch unreachable;
    } else |err| {
        std.debug.print("foo: {}\n", .{err});
        return .{ first, second };
    }
    unreachable;
}

pub fn main() !void {
    var allocator = std.heap.GeneralPurposeAllocator(.{}){};
    const alloc = allocator.allocator();

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
