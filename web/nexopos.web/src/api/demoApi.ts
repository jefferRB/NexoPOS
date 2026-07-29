import { getJson } from './client';
import type {
  BranchDetail,
  Inventory,
  InvoiceDetail,
  InvoiceListResponse,
  Overview,
  ProductDetail,
  ReorderBasisCode,
  ReorderResponse,
  Service,
  MobileKit,
} from '../types/api';

export function fetchOverview(signal?: AbortSignal): Promise<Overview> {
  return getJson<Overview>('/api/demo/overview', signal);
}

export function fetchInventory(signal?: AbortSignal): Promise<Inventory> {
  return getJson<Inventory>('/api/demo/inventory', signal);
}

export function fetchBranchDetail(branchId: string, signal?: AbortSignal): Promise<BranchDetail> {
  return getJson<BranchDetail>(`/api/demo/branches/${encodeURIComponent(branchId)}`, signal);
}

export function fetchProductDetail(productId: string, signal?: AbortSignal): Promise<ProductDetail> {
  return getJson<ProductDetail>(`/api/demo/products/${encodeURIComponent(productId)}`, signal);
}

export function fetchServices(signal?: AbortSignal): Promise<Service[]> {
  return getJson<Service[]>('/api/demo/services', signal);
}

export function fetchMobileKits(signal?: AbortSignal): Promise<MobileKit[]> {
  return getJson<MobileKit[]>('/api/demo/mobile-kits', signal);
}

export function fetchInvoices(signal?: AbortSignal): Promise<InvoiceListResponse> {
  return getJson<InvoiceListResponse>('/api/demo/invoices', signal);
}

export function fetchInvoice(invoiceId: string, signal?: AbortSignal): Promise<InvoiceDetail> {
  return getJson<InvoiceDetail>(`/api/demo/invoices/${encodeURIComponent(invoiceId)}`, signal);
}

export function fetchReorderSuggestions(basis: ReorderBasisCode, signal?: AbortSignal): Promise<ReorderResponse> {
  return getJson<ReorderResponse>(`/api/demo/reorder?basis=${basis}`, signal);
}
