const std = @import("std");
const Allocator = std.mem.Allocator;
const ArrayList = std.ArrayList;

fn parseList(alloc: Allocator, input: []const u8) [2]ArrayList(i32) {
    var lines = std.mem.split(u8, input, "\n");
    var first = ArrayList(i32).init(alloc);
    var second = ArrayList(i32).init(alloc);

    while (lines.next()) |line| {
        var strs = std.mem.split(u8, line, "   ");
        const match: i32 = std.fmt.parseInt(i32, strs.next().?, 10) catch unreachable;
        std.debug.print("look: {any}\n", .{match});
        first.append(match) catch unreachable;
        second.append(std.fmt.parseInt(i32, strs.next().?, 10) catch unreachable) catch unreachable;
    }

    return .{ first, second };
}

pub fn main() !void {
    var allocator = std.heap.GeneralPurposeAllocator(.{}){};
    const alloc = allocator.allocator();

    const input: []const u8 = "3   4\n4   3\n2   5\n1   3\n3   9\n3   3";
    const parsed = parseList(alloc, input);
    std.debug.print("{any}\n", .{parsed[0].items});
    std.mem.sort(i32, parsed[0].items, {}, std.sort.asc(i32));
    std.mem.sort(i32, parsed[1].items, {}, std.sort.asc(i32));
    std.debug.print("{any}\n", .{parsed[0].items});

    const diff = try ArrayList(i32).initCapacity(alloc, parsed[0].items.len);
    for (parsed[0].items, parsed[1].items, 0..) |x, y, i| {
        std.debug.print("{any} {any} {any}\n", .{x, y, i});
        diff.items[i] = @abs(y - x);

    }
    std.debug.print("{any}\n", .{diff.items});
}
