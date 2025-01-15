const std = @import("std");
const dbgPr = std.debug.print;
const AutoHashMap = std.hash_map.AutoHashMap;

const parseList = @import("inputReader.zig").parseList;

pub fn main() !void {
    var arena = std.heap.ArenaAllocator.init(std.heap.page_allocator);
    defer arena.deinit();
    const alloc = arena.allocator();

    const stdin = std.io.getStdIn();
    var buf_reader = std.io.bufferedReader(stdin.reader());
    const parsed = try parseList(alloc, buf_reader.reader().any());
    dbgPr("{any}\n", .{parsed[0].items});

    var count = AutoHashMap(i32, i32).init(alloc);
    for (parsed[0].items) |x| {
        const current = count.get(x) orelse 0;
        try count.put(x, current + 1);
    }

    var keys = count.keyIterator();
    while (keys.next()) |x| {
        dbgPr("{d}:{d} ", .{ x.*, count.get(x.*)? });
    }
    dbgPr("\n", .{});

    dbgPr("{any}\n", .{count});
}
