// Tipos que reflejan los DTOs expuestos por el API (NexoPOS.Application.Demo.Dtos).
// Importes en colones costarricenses (CRC); las fechas llegan como ISO 8601 (UTC).

export type StockStatusCode = 'available' | 'low' | 'out';

export type ProductTypeCode = 'standard' | 'medication' | 'food' | 'clinical-supply';

export type UnitCode = 'unit' | 'ml' | 'kg';

export type ActivityTypeCode =
  | 'invoice-issued'
  | 'fractional-sale'
  | 'service-billed'
  | 'adjustment'
  | 'purchase'
  | 'transfer-out'
  | 'transfer-in'
  | 'transfer-kit'
  | 'receivable-payment'
  | 'cash-closing';

export type AlertCategory = 'stock-low' | 'stock-out' | 'mobile-kit' | 'transfer' | 'reorder';

export type AlertSeverity = 'high' | 'medium' | 'low';

export type MobileKitStatusCode = 'on-route' | 'available' | 'needs-review';

export type InvoiceTypeCode = 'electronic-invoice' | 'electronic-ticket';

export type PaymentMethodCode = 'cash' | 'card' | 'sinpe-movil' | 'bank-transfer' | 'credit' | 'mixed';

export type InvoiceStatusCode = 'accepted' | 'pending' | 'voided';

export type ReorderPriorityCode = 'high' | 'medium' | 'low';

export type ReorderBasisCode = 'weekly' | 'monthly';

export interface Indicators {
  salesToday: number;
  ticketsToday: number;
  lowStockProducts: number;
  reorderSuggestedCount: number;
  receivablesTotal: number;
  payablesTotal: number;
}

export interface BranchSummary {
  id: string;
  name: string;
  location: string;
  phone: string;
  schedule: string;
  isOperational: boolean;
  salesToday: number;
  ticketsToday: number;
  managedProductCount: number;
  lowStockCount: number;
  activeCollaborators: number;
  mobileKitsCount: number;
  receivablesBalance: number;
  payablesBalance: number;
}

export interface PriorityAlert {
  id: string;
  category: AlertCategory;
  title: string;
  description: string;
  severity: AlertSeverity;
  branchId: string | null;
  branchName: string | null;
}

export interface ActivityItem {
  id: string;
  type: ActivityTypeCode;
  action: string;
  userName: string;
  userRole: string;
  locationName: string;
  branchId: string | null;
  mobileKitId: string | null;
  timestamp: string;
  reference: string;
  reason: string;
  amount: number | null;
}

export interface Overview {
  indicators: Indicators;
  branches: BranchSummary[];
  priorityAlerts: PriorityAlert[];
  recentActivity: ActivityItem[];
}

export interface BranchRef {
  id: string;
  name: string;
}

export interface InventoryItem {
  productId: string;
  name: string;
  internalCode: string;
  manufacturerBarcode: string;
  category: string;
  type: ProductTypeCode;
  unit: UnitCode;
  isFractionable: boolean;
  supplierId: string;
  supplierName: string;
  stockByBranch: Record<string, number>;
  mobileKitsStock: number;
  total: number;
  minimum: number;
  status: StockStatusCode;
}

export interface Inventory {
  branches: BranchRef[];
  items: InventoryItem[];
}

export type ReorderStatusCode = 'needs-reorder' | 'sufficient' | 'no-data';

export interface ProductDetail {
  summary: InventoryItem;
  purchaseUnitLabel: string | null;
  baseUnitsPerPurchaseUnit: number | null;
  weeklyAverageSales: number;
  monthlyAverageSales: number;
  coverageDays: number | null;
  reorderStatus: ReorderStatusCode;
}

export interface BranchInventoryItem {
  productId: string;
  name: string;
  internalCode: string;
  type: ProductTypeCode;
  quantity: number;
  unit: UnitCode;
  minimum: number;
  status: StockStatusCode;
}

export interface DailyPerformance {
  date: string;
  tickets: number;
  sales: number;
}

export interface TopProduct {
  productName: string;
  quantitySold: number;
  unit: UnitCode;
}

export interface BranchDetail {
  branch: BranchSummary;
  inventory: BranchInventoryItem[];
  recentActivity: ActivityItem[];
  weeklyPerformance: DailyPerformance[];
  topProducts: TopProduct[];
}

export interface ServiceComponent {
  productId: string | null;
  label: string;
  quantity: number | null;
  unit: UnitCode | null;
  durationMinutes: number | null;
  linksToInventory: boolean;
}

export interface Service {
  id: string;
  name: string;
  description: string;
  durationMinutes: number;
  price: number;
  components: ServiceComponent[];
}

export interface MobileKitStockLine {
  productId: string;
  productName: string;
  quantity: number;
  unit: UnitCode;
  estimatedValue: number;
}

export interface MobileKit {
  id: string;
  name: string;
  assignedTo: string;
  homeBranchId: string;
  homeBranchName: string;
  status: MobileKitStatusCode;
  lastTransferAt: string | null;
  lastConsumptionAt: string | null;
  estimatedValue: number;
  alerts: string[];
  stock: MobileKitStockLine[];
  recentActivity: ActivityItem[];
}

export interface InvoiceLine {
  description: string;
  quantity: number;
  unit: UnitCode;
  unitPrice: number;
  lineTotal: number;
}

export interface InvoiceSummary {
  id: string;
  number: string;
  issuedAt: string;
  branchId: string;
  branchName: string;
  customerName: string;
  issuedBy: string;
  type: InvoiceTypeCode;
  paymentMethod: PaymentMethodCode;
  total: number;
  status: InvoiceStatusCode;
}

export interface InvoiceDetail {
  summary: InvoiceSummary;
  lines: InvoiceLine[];
}

export interface InvoiceIndicators {
  issuedToday: number;
  accepted: number;
  pending: number;
  voided: number;
  averageTicket: number;
}

export interface InvoiceListResponse {
  indicators: InvoiceIndicators;
  invoices: InvoiceSummary[];
}

export interface ReorderSuggestion {
  productId: string;
  productName: string;
  productCode: string;
  supplierId: string;
  supplierName: string;
  currentStock: number;
  unit: UnitCode;
  weeklyAverageSales: number;
  monthlyAverageSales: number;
  coverageDays: number;
  suggestedQuantity: number;
  priority: ReorderPriorityCode;
}

export interface SupplierOrder {
  supplierId: string;
  supplierName: string;
  estimatedValue: number;
  items: ReorderSuggestion[];
}

export interface ReorderIndicators {
  productsToReorder: number;
  suppliersInvolved: number;
  estimatedValue: number;
  averageCoverageDays: number;
}

export interface ReorderResponse {
  basis: ReorderBasisCode;
  indicators: ReorderIndicators;
  supplierOrders: SupplierOrder[];
}
